namespace CommonDTO;


    
public class PluginDTO
{
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public string Category { get; set; }
    
    public bool Enabled { get; set; }
    
    public bool Premium { get; set; }
}
