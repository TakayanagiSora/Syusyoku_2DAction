
public static class Wrapper
{
    /// <summary>
    /// GameObject‚ÌClone•\‹L‚ğíœ‚·‚é
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static string OriginalizeTheName(string original)
    {
        return original.Replace("(Clone)", "");
    }
}
