import SwiftUI

struct RootView: View {
    var body: some View {
        TabView {
            ProductsView()
                .tabItem { Label("Products", systemImage: "shippingbox.fill") }

            LocationsView()
                .tabItem { Label("Locations", systemImage: "mappin.and.ellipse") }

            SettingsView()
                .tabItem { Label("Settings", systemImage: "gearshape.fill") }
        }
        .tint(Palette.accentGlow)
    }
}
