import SwiftUI

enum LocationFormMode: Identifiable {
    case create
    case edit(Location)

    var id: String {
        switch self {
        case .create: return "create"
        case let .edit(location): return String(location.id)
        }
    }
}

struct LocationsView: View {
    @Environment(API.self) private var api

    @State private var locations: [Location] = []
    @State private var isLoading = false
    @State private var loadedOnce = false
    @State private var error: String?
    @State private var currentLoad: UUID?
    @State private var form: LocationFormMode?

    var body: some View {
        NavigationStack {
            ZStack {
                AppBackground()
                content
            }
            .navigationTitle("Locations")
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button { form = .create } label: { Image(systemName: "plus") }
                }
            }
            .sheet(item: $form) { mode in
                LocationFormView(mode: mode) { await reload() }
                    .environment(api)
            }
            .task {
                guard !loadedOnce else { return }
                loadedOnce = true
                await reload()
            }
        }
    }

    @ViewBuilder
    private var content: some View {
        List {
            Text("Where your things are kept.")
                .font(.system(.subheadline, design: .rounded))
                .foregroundStyle(Palette.muted)
                .clearListRow()

            if isLoading && locations.isEmpty {
                LoadingStateView(label: "Loading locations…").clearListRow()
            } else if let error, locations.isEmpty {
                ConnectionErrorView(message: error) { Task { await reload() } }.clearListRow()
            } else if locations.isEmpty {
                EmptyStateView(icon: "mappin.and.ellipse",
                               message: "No locations yet. Tap + to add one, e.g. “Drawer in the wardrobe”.")
                    .clearListRow()
            } else {
                if let error {
                    InlineErrorBanner(message: error).clearListRow()
                }
                ForEach(locations) { location in
                    locationRow(location)
                        .contentShape(Rectangle())
                        .onTapGesture { form = .edit(location) }
                        .clearListRow()
                        .swipeActions(edge: .trailing, allowsFullSwipe: false) {
                            Button(role: .destructive) {
                                Task { await delete(location) }
                            } label: {
                                Image(systemName: "trash")
                            }
                            .tint(Palette.danger)
                        }
                }
            }
        }
        .listStyle(.plain)
        .scrollContentBackground(.hidden)
        .scrollBounceBehavior(.always)
        .refreshable { await reload() }
    }

    private func locationRow(_ location: Location) -> some View {
        HStack(spacing: 12) {
            Image(systemName: "mappin.circle.fill")
                .font(.title3)
                .foregroundStyle(Palette.accentGlow)
            Text(location.locationName)
                .font(.system(.body, design: .rounded).weight(.medium))
                .foregroundStyle(Palette.text)
            Spacer(minLength: 8)
            Image(systemName: "chevron.right")
                .font(.footnote.weight(.semibold))
                .foregroundStyle(Palette.muted)
        }
        .cardStyle(padding: 14)
    }

    private func reload() async {
        let token = UUID()
        currentLoad = token
        isLoading = true
        defer { if currentLoad == token { isLoading = false } }

        do {
            let locations = try await api.locations()
            guard currentLoad == token else { return }
            self.locations = locations
            self.error = nil
        } catch is CancellationError {
        } catch let urlError as URLError where urlError.code == .cancelled {
        } catch {
            guard currentLoad == token else { return }
            self.error = error.localizedDescription
        }
    }

    private func delete(_ location: Location) async {
        do {
            try await api.deleteLocation(id: location.id)
            await reload()
        } catch {
            // Surfaces the API's 409 "still has products assigned" message.
            self.error = error.localizedDescription
        }
    }
}
