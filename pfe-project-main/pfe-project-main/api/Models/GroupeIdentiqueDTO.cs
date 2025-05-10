using System;
using System.ComponentModel.DataAnnotations;
public class GroupeIdentiqueDTO
{
    public int Id { get; set; }
    public string Designation { get; set; } = string.Empty;
    public string MarqueNom { get; set; } = string.Empty;
    public string TypeEquipNom { get; set; } = string.Empty;

    public List<string> Organes { get; set; } = new();
    public List<string> Caracteristiques { get; set; } = new();
}
