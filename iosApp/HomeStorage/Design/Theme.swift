import SwiftUI

// Design tokens ported from the Blazor web app (wwwroot/app.css).
enum Palette {
    static let bg            = Color(hex: 0x070D1F)
    static let bgGradTop     = Color(hex: 0x0A1330)
    static let surface       = Color(hex: 0x0F1A38)
    static let surface2      = Color(hex: 0x14224A)
    static let border        = Color(hex: 0x7D9BDC, alpha: 0.14)
    static let borderStrong  = Color(hex: 0x7D9BDC, alpha: 0.30)

    static let text          = Color(hex: 0xE7EDFF)
    static let muted         = Color(hex: 0x8BA0CC)

    static let accent        = Color(hex: 0x3B82F6)
    static let accentStrong  = Color(hex: 0x2563EB)
    static let accentGlow    = Color(hex: 0x38BDF8)
    static let danger        = Color(hex: 0xF0556D)
    static let dangerStrong  = Color(hex: 0xD83A55)
    static let success       = Color(hex: 0x34D399)

    static let pillText      = Color(hex: 0xCFE4FF)
    static let locText       = Color(hex: 0x9DDCFF)
}

extension Color {
    init(hex: UInt, alpha: Double = 1) {
        self.init(.sRGB,
                  red: Double((hex >> 16) & 0xFF) / 255,
                  green: Double((hex >> 8) & 0xFF) / 255,
                  blue: Double(hex & 0xFF) / 255,
                  opacity: alpha)
    }
}

// MARK: - Background

struct AppBackground: View {
    var body: some View {
        ZStack {
            LinearGradient(colors: [Palette.bgGradTop, Palette.bg],
                           startPoint: .top, endPoint: .center)
            RadialGradient(colors: [Palette.accent.opacity(0.16), .clear],
                           center: UnitPoint(x: 0.12, y: -0.05), startRadius: 0, endRadius: 620)
            RadialGradient(colors: [Palette.accentGlow.opacity(0.10), .clear],
                           center: UnitPoint(x: 1.0, y: 0.0), startRadius: 0, endRadius: 520)
        }
        .background(Palette.bg)
        .ignoresSafeArea()
    }
}

// MARK: - Cards

struct CardStyle: ViewModifier {
    var padding: CGFloat = 18
    func body(content: Content) -> some View {
        content
            .padding(padding)
            .background(Palette.surface, in: RoundedRectangle(cornerRadius: 14, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 14, style: .continuous).stroke(Palette.border, lineWidth: 1))
            .shadow(color: .black.opacity(0.45), radius: 16, x: 0, y: 10)
    }
}

extension View {
    func cardStyle(padding: CGFloat = 18) -> some View { modifier(CardStyle(padding: padding)) }

    /// Transparent List row so the themed background shows through.
    func clearListRow() -> some View {
        listRowBackground(Color.clear)
            .listRowSeparator(.hidden)
            .listRowInsets(EdgeInsets(top: 8, leading: 16, bottom: 8, trailing: 16))
    }
}

// MARK: - Buttons

struct PrimaryButtonStyle: ButtonStyle {
    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .font(.system(.body, design: .rounded).weight(.semibold))
            .foregroundStyle(.white)
            .padding(.vertical, 10)
            .padding(.horizontal, 18)
            .background(
                LinearGradient(colors: [Palette.accent, Palette.accentStrong],
                               startPoint: .top, endPoint: .bottom),
                in: RoundedRectangle(cornerRadius: 10, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 10, style: .continuous).stroke(.white.opacity(0.18)))
            .shadow(color: Palette.accent.opacity(0.5), radius: 12, y: 6)
            .opacity(configuration.isPressed ? 0.85 : 1)
            .scaleEffect(configuration.isPressed ? 0.99 : 1)
    }
}

struct SecondaryButtonStyle: ButtonStyle {
    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .font(.system(.subheadline, design: .rounded).weight(.medium))
            .foregroundStyle(Palette.text)
            .padding(.vertical, 8)
            .padding(.horizontal, 14)
            .background(Color(hex: 0x7D9BDC, alpha: 0.10), in: RoundedRectangle(cornerRadius: 10, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 10, style: .continuous).stroke(Palette.borderStrong))
            .opacity(configuration.isPressed ? 0.8 : 1)
    }
}

struct DangerButtonStyle: ButtonStyle {
    func makeBody(configuration: Configuration) -> some View {
        configuration.label
            .font(.system(.subheadline, design: .rounded).weight(.medium))
            .foregroundStyle(Color(hex: 0xFF8198))
            .padding(.vertical, 8)
            .padding(.horizontal, 14)
            .background(Color.clear, in: RoundedRectangle(cornerRadius: 10, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 10, style: .continuous).stroke(Palette.danger.opacity(0.5)))
            .opacity(configuration.isPressed ? 0.8 : 1)
    }
}

// MARK: - Form fields

struct FieldLabel: View {
    let text: String
    var body: some View {
        Text(text.uppercased())
            .font(.system(.caption2, design: .rounded).weight(.semibold))
            .tracking(0.6)
            .foregroundStyle(Palette.muted)
    }
}

struct FieldBox: ViewModifier {
    func body(content: Content) -> some View {
        content
            .padding(.vertical, 11)
            .padding(.horizontal, 13)
            .background(Palette.surface2, in: RoundedRectangle(cornerRadius: 10, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 10, style: .continuous).stroke(Palette.borderStrong))
            .foregroundStyle(Palette.text)
            .tint(Palette.accentGlow)
    }
}

extension View {
    func fieldBox() -> some View { modifier(FieldBox()) }
}

// MARK: - Small components

struct QtyPill: View {
    let value: Double
    var body: some View {
        Text(value.formattedQuantity)
            .font(.system(.caption, design: .rounded).weight(.semibold))
            .foregroundStyle(Palette.pillText)
            .padding(.vertical, 3)
            .padding(.horizontal, 10)
            .background(Palette.accent.opacity(0.18), in: Capsule())
            .overlay(Capsule().stroke(Palette.accent.opacity(0.4)))
    }
}

struct LocTag: View {
    let name: String
    var body: some View {
        Text(name)
            .font(.system(.caption, design: .rounded))
            .foregroundStyle(Palette.locText)
            .padding(.vertical, 2)
            .padding(.horizontal, 8)
            .background(Palette.accentGlow.opacity(0.10), in: RoundedRectangle(cornerRadius: 8, style: .continuous))
            .overlay(RoundedRectangle(cornerRadius: 8, style: .continuous).stroke(Palette.accentGlow.opacity(0.28)))
    }
}

struct MetaLabel: View {
    let text: String
    var body: some View {
        Text(text.uppercased())
            .font(.system(.caption2, design: .rounded).weight(.semibold))
            .tracking(0.6)
            .foregroundStyle(Palette.muted)
            .frame(width: 84, alignment: .leading)
    }
}

struct EmptyStateView: View {
    let icon: String
    let message: String
    var body: some View {
        VStack(spacing: 12) {
            Image(systemName: icon)
                .font(.system(size: 38))
                .foregroundStyle(Palette.muted)
            Text(message)
                .font(.system(.body, design: .rounded))
                .foregroundStyle(Palette.muted)
                .multilineTextAlignment(.center)
        }
        .frame(maxWidth: .infinity)
        .padding(.vertical, 48)
        .padding(.horizontal, 20)
        .background(Color(hex: 0x7D9BDC, alpha: 0.03), in: RoundedRectangle(cornerRadius: 14, style: .continuous))
        .overlay(
            RoundedRectangle(cornerRadius: 14, style: .continuous)
                .strokeBorder(style: StrokeStyle(lineWidth: 1, dash: [6, 5]))
                .foregroundStyle(Palette.borderStrong)
        )
    }
}

struct LoadingStateView: View {
    var label: String = "Loading…"
    var body: some View {
        VStack(spacing: 14) {
            ProgressView().tint(Palette.accentGlow).controlSize(.large)
            Text(label)
                .font(.system(.subheadline, design: .rounded))
                .foregroundStyle(Palette.muted)
        }
        .frame(maxWidth: .infinity)
        .padding(.vertical, 60)
    }
}

/// Prominent, themed state shown when the API can't be reached (offline, timeout, …).
struct ConnectionErrorView: View {
    var title: String = "Can't reach the server"
    let message: String
    var retry: () -> Void

    var body: some View {
        VStack(spacing: 18) {
            ZStack {
                Circle().fill(Palette.danger.opacity(0.12)).frame(width: 76, height: 76)
                Circle().stroke(Palette.danger.opacity(0.35), lineWidth: 1).frame(width: 76, height: 76)
                Image(systemName: "wifi.exclamationmark")
                    .font(.system(size: 30, weight: .semibold))
                    .foregroundStyle(Palette.danger)
            }

            VStack(spacing: 7) {
                Text(title)
                    .font(.system(.title3, design: .rounded).weight(.semibold))
                    .foregroundStyle(.white)
                Text(message)
                    .font(.system(.subheadline, design: .rounded))
                    .foregroundStyle(Palette.muted)
                    .multilineTextAlignment(.center)
            }

            Button(action: retry) {
                Label("Try again", systemImage: "arrow.clockwise")
            }
            .buttonStyle(PrimaryButtonStyle())

            Text("Pull down to refresh")
                .font(.caption)
                .foregroundStyle(Palette.muted.opacity(0.7))
        }
        .frame(maxWidth: .infinity)
        .padding(.vertical, 40)
        .padding(.horizontal, 24)
        .background(Palette.surface, in: RoundedRectangle(cornerRadius: 16, style: .continuous))
        .overlay(RoundedRectangle(cornerRadius: 16, style: .continuous).stroke(Palette.danger.opacity(0.25)))
        .shadow(color: .black.opacity(0.4), radius: 18, y: 10)
    }
}

/// Compact inline banner shown when a refresh fails but stale data is still on screen.
struct InlineErrorBanner: View {
    let message: String
    var body: some View {
        HStack(spacing: 10) {
            Image(systemName: "exclamationmark.triangle.fill")
                .foregroundStyle(Palette.danger)
            Text(message)
                .font(.footnote)
                .foregroundStyle(Palette.text)
            Spacer(minLength: 0)
        }
        .padding(12)
        .frame(maxWidth: .infinity, alignment: .leading)
        .background(Palette.danger.opacity(0.10), in: RoundedRectangle(cornerRadius: 10, style: .continuous))
        .overlay(RoundedRectangle(cornerRadius: 10, style: .continuous).stroke(Palette.danger.opacity(0.3)))
    }
}
