import SwiftUI
import UIKit

enum Keyboard {
    static func dismiss() {
        UIApplication.shared.sendAction(#selector(UIResponder.resignFirstResponder), to: nil, from: nil, for: nil)
    }
}

extension View {
    /// Dismisses the keyboard when the user taps empty space.
    /// Taps on text fields, pickers and buttons are still delivered to them first.
    func dismissKeyboardOnTap() -> some View {
        contentShape(Rectangle())
            .onTapGesture { Keyboard.dismiss() }
    }
}
