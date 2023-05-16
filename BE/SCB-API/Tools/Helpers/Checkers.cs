namespace SCB_API.Tools.Helpers
{
    public class Checkers
    {
        /// <summary>
        /// Check if <paramref name="val"/> is null or undefined
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsNullOrUndefined(string val)
        {
            return val == null || val == "undefined";
        }
    }
}
