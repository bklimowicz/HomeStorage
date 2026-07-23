import Foundation
import Observation

/// Thin async/await client for the HomeStorage API. The base URL is
/// user-configurable (Settings tab) and persisted in UserDefaults so the same
/// build can point at `http://localhost:5080` (simulator) or the public
/// Cloudflare Tunnel hostname.
@Observable
final class API {
    static let defaultBaseURL = "http://localhost:5080"
    private static let storageKey = "apiBaseURL"

    var baseURLString: String {
        didSet { UserDefaults.standard.set(baseURLString, forKey: Self.storageKey) }
    }

    init() {
        baseURLString = UserDefaults.standard.string(forKey: Self.storageKey) ?? Self.defaultBaseURL
    }

    // ---- Products ----
    func products() async throws -> [Product] { try await get("/products") }
    func createProduct(_ body: ProductPayload) async throws { try await send("POST", "/products", body) }
    func updateProduct(id: String, _ body: ProductPayload) async throws { try await send("PUT", "/products/\(id)", body) }
    func deleteProduct(id: String) async throws { try await send("DELETE", "/products/\(id)", noBody) }

    // ---- Locations ----
    func locations() async throws -> [Location] { try await get("/locations") }
    func createLocation(_ body: LocationPayload) async throws { try await send("POST", "/locations", body) }
    func updateLocation(id: Int, _ body: LocationPayload) async throws { try await send("PUT", "/locations/\(id)", body) }
    func deleteLocation(id: Int) async throws { try await send("DELETE", "/locations/\(id)", noBody) }

    // ---- Plumbing ----
    private let noBody: ProductPayload? = nil

    private func get<T: Decodable>(_ path: String) async throws -> T {
        let (data, response) = try await URLSession.shared.data(for: try request(path, "GET"))
        try validate(response, data)
        do {
            return try JSONDecoder().decode(T.self, from: data)
        } catch {
            throw APIError.decoding
        }
    }

    private func send<Body: Encodable>(_ method: String, _ path: String, _ body: Body?) async throws {
        var req = try request(path, method)
        if let body {
            req.httpBody = try JSONEncoder().encode(body)
            req.setValue("application/json", forHTTPHeaderField: "Content-Type")
        }
        let (data, response) = try await URLSession.shared.data(for: req)
        try validate(response, data)
    }

    private func request(_ path: String, _ method: String) throws -> URLRequest {
        guard let base = URL(string: baseURLString.trimmed),
              let url = URL(string: path, relativeTo: base) else {
            throw APIError.badURL
        }
        var req = URLRequest(url: url)
        req.httpMethod = method
        req.timeoutInterval = 20
        return req
    }

    private func validate(_ response: URLResponse, _ data: Data) throws {
        guard let http = response as? HTTPURLResponse else { throw APIError.invalidResponse }
        guard (200..<300).contains(http.statusCode) else {
            let message = (try? JSONDecoder().decode(ServerMessage.self, from: data))?.message
            throw APIError.server(status: http.statusCode, message: message)
        }
    }
}

private struct ServerMessage: Decodable { let message: String? }

enum APIError: LocalizedError {
    case badURL
    case invalidResponse
    case decoding
    case server(status: Int, message: String?)

    var errorDescription: String? {
        switch self {
        case .badURL: return "Invalid API base URL — check it in Settings."
        case .invalidResponse: return "Unexpected response from the server."
        case .decoding: return "Could not read the server response."
        case let .server(status, message): return message ?? "Request failed (HTTP \(status))."
        }
    }
}
