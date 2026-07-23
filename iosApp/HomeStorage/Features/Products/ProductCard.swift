import SwiftUI

struct ProductCard: View {
    let product: Product

    var body: some View {
        HStack(alignment: .center, spacing: 12) {
            VStack(alignment: .leading, spacing: 12) {
                HStack(alignment: .firstTextBaseline) {
                    Text(product.name)
                        .font(.system(.headline, design: .rounded).weight(.semibold))
                        .foregroundStyle(.white)
                    Spacer(minLength: 8)
                    QtyPill(value: product.quantity)
                }

                if let description = product.description, !description.isEmpty {
                    Text(description)
                        .font(.subheadline)
                        .foregroundStyle(Palette.muted)
                }

                if let producer = product.producer, !producer.isEmpty {
                    HStack(spacing: 4) {
                        MetaLabel(text: "Producer")
                        Text(producer)
                            .font(.footnote)
                            .foregroundStyle(Palette.text)
                    }
                }

                HStack(spacing: 4) {
                    MetaLabel(text: "Location")
                    LocTag(name: product.locationName ?? "—")
                }
            }

            Image(systemName: "chevron.right")
                .font(.footnote.weight(.semibold))
                .foregroundStyle(Palette.muted)
        }
        .frame(maxWidth: .infinity, alignment: .leading)
        .cardStyle()
    }
}
