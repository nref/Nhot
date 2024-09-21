namespace Nhot;

public static class KeyModifierExtensions
{
    public static uint Flatten(this KeyModifier[] ModifierKeys)
    {
        uint modifiers = 0;
        foreach (var key in ModifierKeys)
        {
            modifiers |= (uint)key;
        }
        return modifiers;
    }
}
