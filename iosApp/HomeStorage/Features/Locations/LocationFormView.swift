import SwiftUI

struct LocationFormView: View {
    @Environment(API.self) private var api
    @Environment(\.dismiss) private var dismiss

    let mode: LocationFormMode
    var onSaved: () async -> Void

    @State private var name = ""
    @State private var error: String?
    @State private var isSaving = false

    private var isEditing: Bool {
        if case .edit = mode { return true }
        return false
    }

    var body: some View {
        NavigationStack {
            ZStack {
                AppBackground()
                ScrollView {
                    VStack(alignment: .leading, spacing: 18) {
                        VStack(alignment: .leading, spacing: 7) {
                            FieldLabel(text: "Name")
                            TextField("e.g. Drawer in the wardrobe", text: $name).fieldBox()
                        }

                        if let error {
                            Text(error)
                                .font(.footnote)
                                .foregroundStyle(Palette.danger)
                        }

                        Button {
                            Task { await save() }
                        } label: {
                            Text(isSaving ? "Saving…" : "Save")
                                .frame(maxWidth: .infinity)
                        }
                        .buttonStyle(PrimaryButtonStyle())
                        .disabled(isSaving)
                        .padding(.top, 4)
                    }
                    .cardStyle()
                    .padding(20)
                }
            }
            .scrollDismissesKeyboard(.interactively)
            .dismissKeyboardOnTap()
            .navigationTitle(isEditing ? "Edit Location" : "New Location")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Cancel") { dismiss() }
                }
            }
            .onAppear(perform: prime)
        }
    }

    private func prime() {
        guard case let .edit(location) = mode else { return }
        name = location.locationName
    }

    private func save() async {
        guard !name.trimmed.isEmpty else { error = "Name is required."; return }

        isSaving = true
        error = nil
        let payload = LocationPayload(locationName: name.trimmed)
        do {
            switch mode {
            case .create:
                try await api.createLocation(payload)
            case let .edit(location):
                try await api.updateLocation(id: location.id, payload)
            }
            await onSaved()
            dismiss()
        } catch {
            self.error = error.localizedDescription
        }
        isSaving = false
    }
}
