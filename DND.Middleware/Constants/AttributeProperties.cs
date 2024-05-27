namespace DND.Middleware.Constants
{
    public class MinLengths
    {
        public const int Text = 1;
        public const int TinyText = 1;
        public const int ShortText = 1;
        public const int LongText = 1;
        public const int DescriptiveText = 1;
        public const int PasswordText = 8;
    }

    public class MaxLengths
    {
        public const int Text = 128;
        public const int TinyText = 32;
        public const int ShortText = 64;
        public const int LongText = 256;
        public const int DescriptiveText = 512;
        public const int PasswordText = 128;
    }
}
