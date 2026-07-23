import Foundation

extension String {
    var trimmed: String { trimmingCharacters(in: .whitespacesAndNewlines) }
    var nilIfEmpty: String? { trimmed.isEmpty ? nil : trimmed }
}

extension Double {
    /// "5" instead of "5.0", "2.5" stays "2.5".
    var formattedQuantity: String {
        self == rounded() ? String(Int(self)) : String(format: "%g", self)
    }
}
