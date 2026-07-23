import Foundation

// Mirrors the API's ProductDto (System.Text.Json camelCase).
struct Product: Identifiable, Codable, Hashable {
    let id: String          // GUID serialized as a string
    var name: String
    var quantity: Double
    var locationId: Int
    var locationName: String?
    var description: String?
    var producer: String?
}

// Mirrors the API's LocationDto.
struct Location: Identifiable, Codable, Hashable {
    let id: Int
    var locationName: String
}

// ---- Request bodies (match CreateProduct / UpdateProduct / *Location) ----

struct ProductPayload: Codable {
    var name: String
    var quantity: Double
    var locationId: Int
    var description: String?
    var producer: String?
}

struct LocationPayload: Codable {
    var locationName: String
}
