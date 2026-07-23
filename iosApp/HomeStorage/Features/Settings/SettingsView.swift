import SwiftUI

struct SettingsView: View {
    @Environment(API.self) private var api

    var body: some View {
        @Bindable var api = api

        NavigationStack {
            ZStack {
                AppBackground()
                ScrollView {
                    VStack(alignment: .leading, spacing: 18) {
                        VStack(alignment: .leading, spacing: 7) {
                            FieldLabel(text: "API base URL")
                            TextField(API.defaultBaseURL, text: $api.baseURLString)
                                .keyboardType(.URL)
                                .textInputAutocapitalization(.never)
                                .autocorrectionDisabled()
                                .fieldBox()
                            Text("Where the HomeStorage API is reachable. Use http://localhost:5080 in the "
                                 + "simulator, or your Cloudflare Tunnel hostname (e.g. https://api.yourdomain.com).")
                                .font(.footnote)
                                .foregroundStyle(Palette.muted)
                        }

                        Button {
                            api.baseURLString = API.defaultBaseURL
                        } label: {
                            Text("Reset to default")
                        }
                        .buttonStyle(SecondaryButtonStyle())
                    }
                    .cardStyle()
                    .padding(20)
                }
            }
            .scrollDismissesKeyboard(.interactively)
            .dismissKeyboardOnTap()
            .navigationTitle("Settings")
        }
    }
}
