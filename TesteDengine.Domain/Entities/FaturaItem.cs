namespace TesteDengine;

public class FaturaItem 
{
    public int FaturaItemId { get; set; }
    public int FaturaId { get; set; }
    public int Ordem { get; set; }
    public double Valor { get; set; }
    
    public virtual Fatura Fatura { get; set; }

    public FaturaItem()
    {
    }
}