using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Employé : MonoBehaviour
{
    public abstract float GetSalaire();

    public abstract string ToString();


    public string Matricule;
    public string Nom;
    public string prenom;
    public string Date_De_Naissance;

    public Employé(string Matricule, string Nom, string Prenom, string Date_De_Naissance)
    {
        this.Matricule = Matricule;
        this.Nom = Nom;
        this.prenom = Prenom;
        this.Date_De_Naissance = Date_De_Naissance;
    }
    

}
