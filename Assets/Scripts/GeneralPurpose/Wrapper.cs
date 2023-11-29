
public static class Wrapper
{
    /// <summary>
    /// GameObjectのClone表記を削除する
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string OriginalizeTheName(string original)
    {
        return original.Replace("(Clone)", "");
    }
}
