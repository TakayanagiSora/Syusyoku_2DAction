
public static class Wrapper
{
    /// <summary>
    /// GameObject��Clone�\�L���폜����
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string OriginalizeTheName(string original)
    {
        return original.Replace("(Clone)", "");
    }
}
