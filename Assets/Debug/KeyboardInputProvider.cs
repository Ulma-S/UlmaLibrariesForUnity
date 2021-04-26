using UnityEngine;

public class KeyboardInputProvider : IInputProvider {
    public float HorizontalInput => Input.GetAxisRaw("Horizontal");
}
