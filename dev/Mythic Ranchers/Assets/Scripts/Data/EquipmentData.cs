
/*******************************************************************************

   Nom du fichier: EquipementData.cs
   
   Contexte: Cette classe représente les stats d'un item dans le jeu
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class EquipmentData
{
    private string name;
    private int slot;
    private int stamina;
    private int strength;
    private int intellect;
    private int agility;
    private int armor;
    private int haste;
    private int leech;

    public EquipmentData(string name, int slot, int stamina, int strength, int intellect, int agility, int armor, int haste, int leech)
    {
        this.name = name;
        this.slot = slot;
        this.stamina = stamina;
        this.strength = strength;
        this.intellect = intellect;
        this.agility = agility;
        this.armor = armor;
        this.haste = haste;
        this.leech = leech;
    }

    public string Name { get => name; set => name = value; }
    public int Slot { get => slot; set => slot = value; }
    public int Stamina { get => stamina; set => stamina = value; }
    public int Strength { get => strength; set => strength = value; }
    public int Intellect { get => intellect; set => intellect = value; }
    public int Agility { get => agility; set => agility = value; }
    public int Armor { get => armor; set => armor = value; }
    public int Haste { get => haste; set => haste = value; }
    public int Leech { get => leech; set => leech = value; }
}
