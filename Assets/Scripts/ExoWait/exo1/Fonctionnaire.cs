using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fonctionnaire : Employé
{
    public Fonctionnaire(string Matricule, string Nom, string Prenom, string Date_De_Naissance) : base(Matricule, Nom, Prenom, Date_De_Naissance)
    {
    }

    public override float GetSalaire()
    {
        throw new System.NotImplementedException();
    }
}
