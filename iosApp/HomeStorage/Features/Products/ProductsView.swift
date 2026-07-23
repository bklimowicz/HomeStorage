import SwiftUI

enum ProductFormMode: Identifiable {
    case create
    case edit(Product)

    var id: String {
        switch self {
        case .create: return "create"
        case let .edit(product): return product.id
        }
    }
}

struct ProductsView: View {
    @Environment(API.self) private var api

    @State private var products: [Product] = []
    @State private var locations: [Location] = []
    @State private var isLoading = false
    @State private var loadedOnce = false
    @State private var error: String?
    @State private var currentLoad: UUID?
    @State private var form: ProductFormMode?

    var body: some View {
        NavigationStack {
            ZStack {
                AppBackground()
                content
            }
            .navigationTitle("Products")
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button { form = .create } label: { Image(systemName: "plus") }
                        .disabled(locations.isEmpty)
                }
            }
            .sheet(item: $form) { mode in
                ProductFormView(mode: mode, locations: locations) { await reload() }
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
            Text("Everything you keep at home, and how much of it.")
                .font(.system(.subheadline, design: .rounded))
                .foregroundStyle(Palette.muted)
                .clearListRow()

            if isLoading && products.isEmpty {
                LoadingStateView(label: "Loading products…").clearListRow()
            } else if let error, products.isEmpty {
                ConnectionErrorView(message: error) { Task { await reload() } }.clearListRow()
            } else if locations.isEmpty {
                EmptyStateView(icon: "mappin.slash",
                               message: "Add a location first — every product needs somewhere to live.")
                    .clearListRow()
            } else if products.isEmpty {
                EmptyStateView(icon: "shippingbox",
                               message: "No products yet. Tap + to add your first one.")
                    .clearListRow()
            } else {
                if let error {
                    InlineErrorBanner(message: error).clearListRow()
                }
                ForEach(products) { product in
                    ProductCard(product: product)
                        .contentShape(Rectangle())
                        .onTapGesture { form = .edit(product) }
                        .clearListRow()
                        .swipeActions(edge: .trailing, allowsFullSwipe: false) {
                            Button(role: .destructive) {
                                Task { await delete(product) }
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

    /// Reloads products + locations. Safe to call repeatedly; each call
    /// supersedes the previous one so a stuck request never blocks a refresh.
    private func reload() async {
        let token = UUID()
        currentLoad = token
        isLoading = true
        defer { if currentLoad == token { isLoading = false } }

        do {
            let products = try await api.products()
            let locations = try await api.locations()
            guard currentLoad == token else { return }
            self.products = products
            self.locations = locations
            self.error = nil
        } catch is CancellationError {
        } catch let urlError as URLError where urlError.code == .cancelled {
        } catch {
            guard currentLoad == token else { return }
            self.error = error.localizedDescription
        }
    }

    private func delete(_ product: Product) async {
        do {
            try await api.deleteProduct(id: product.id)
            await reload()
        } catch {
            self.error = error.localizedDescription
        }
    }
}
