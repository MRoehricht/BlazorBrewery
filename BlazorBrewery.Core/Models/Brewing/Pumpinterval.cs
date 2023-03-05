namespace BlazorBrewery.Core.Models.Brewing
{
    public class Pumpinterval
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int RuntimeSeconds { get; set; }
        public int PausetimeSeconds { get; set; }
        public int RuntimeMilliSeconds => RuntimeSeconds * 1000;
        public int PausetimeMilliSeconds => PausetimeSeconds * 1000;

        public override string ToString()
        {
            return Name;
        }
    }
}
