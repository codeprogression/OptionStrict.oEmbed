namespace OptionStrict.oEmbed
{
    public class ValidationRule
    {
        private readonly string _error;
        private readonly string _name;

        public ValidationRule(string name, string error)
        {
            _name = name;
            _error = error;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Error
        {
            get { return _error; }
        }
    }
}