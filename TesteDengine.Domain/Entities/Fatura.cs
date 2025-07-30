using TesteDengine.Domain.Entities;

namespace TesteDengine;

public class Fatura
{
    public int FaturaId { get; set; }
    public int ClienteId { get; set; } 
    public Cliente Cliente { get; set; } 
    public DateTime Data { get; set; }

    public virtual ICollection<FaturaItem> FaturaItem { get; set; }
        
    public Fatura()
    {
        FaturaItem = new HashSet<FaturaItem>();
    }
}