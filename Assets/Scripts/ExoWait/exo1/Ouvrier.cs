using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouvrier : Employé
{
    public int Anciennetee;
    public float SMIG;
    public float Salaire;

    public Ouvrier(string Matricule, string Nom, string Prenom, string Date_De_Naissance, int anciennetee, float SMIG, float salaire) 
        : base(Matricule, Nom, Prenom, Date_De_Naissance)
    {
        this.Matricule = Matricule;
        this.Nom = Nom;
        this.prenom = Prenom;
        this.Date_De_Naissance = Date_De_Naissance;
        this.Anciennetee = anciennetee;
        this.SMIG = SMIG;
        this.Salaire = salaire;
    }

    public override float GetSalaire()
    {
        Salaire += SMIG + (Anciennetee) * 100;
        return Salaire;
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
