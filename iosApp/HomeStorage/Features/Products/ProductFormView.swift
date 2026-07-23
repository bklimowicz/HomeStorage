import SwiftUI

struct ProductFormView: View {
    @Environment(API.self) private var api
    @Environment(\.dismiss) private var dismiss

    let mode: ProductFormMode
    let locations: [Location]
    var onSaved: () async -> Void

    @State private var name = ""
    @State private var description = ""
    @State private var producer = ""
    @State private var quantity = ""
    @State private var locationId: Int?
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
                        labeled("Name") {
                            TextField("e.g. Printer paper", text: $name).fieldBox()
                        }
                        labeled("Description") {
                            TextField("e.g. A4 80g", text: $description).fieldBox()
                        }
                        labeled("Producer") {
                            TextField("e.g. Acme", text: $producer).fieldBox()
                        }
                        labeled("Quantity") {
                            TextField("0", text: $quantity)
                                .keyboardType(.decimalPad)
                                .fieldBox()
                        }
                        labeled("Location") {
                            Picker(selection: $locationId) {
                                Text("Select…").tag(Int?.none)
                                ForEach(locations) { location in
                                    Text(location.locationName).tag(Int?.some(location.id))
                                }
                            } label: { EmptyView() }
                            .pickerStyle(.menu)
                            .tint(Palette.accentGlow)
                            .frame(maxWidth: .infinity, alignment: .leading)
                            .fieldBox()
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
            .navigationTitle(isEditing ? "Edit Product" : "New Product")
            .navigationBarTitleDisplayMode(.inline)
            .toolbar {
                ToolbarItem(placement: .cancellationAction) {
                    Button("Cancel") { dismiss() }
                }
            }
            .onAppear(perform: prime)
        }
    }

    @ViewBuilder
    private func labeled<Content: View>(_ title: String, @ViewBuilder _ content: () -> Content) -> some View {
        VStack(alignment: .leading, spacing: 7) {
            FieldLabel(text: title)
            content()
        }
    }

    private func prime() {
        guard case let .edit(product) = mode else { return }
        name = product.name
        description = product.description ?? ""
        producer = product.producer ?? ""
        quantity = product.quantity.formattedQuantity
        locationId = product.locationId
    }

    private func save() async {
        guard !name.trimmed.isEmpty else { error = "Name is required."; return }
        guard let value = Double(quantity.replacingOccurrences(of: ",", with: ".")), value > 0 else {
            error = "Quantity must be greater than 0."
            return
        }
        guard let locationId else { error = "Please select a location."; return }

        isSaving = true
        error = nil
        let payload = ProductPayload(
            name: name.trimmed,
            quantity: value,
            locationId: locationId,
            description: description.nilIfEmpty,
            producer: producer.nilIfEmpty
        )
        do {
            switch mode {
            case .create:
                try await api.createProduct(payload)
            case let .edit(product):
                try await api.updateProduct(id: product.id, payload)
            }
            await onSaved()
            dismiss()
        } catch {
            self.error = error.localizedDescription
        }
        isSaving = false
    }
}
