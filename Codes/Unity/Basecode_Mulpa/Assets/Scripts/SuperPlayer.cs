using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;  // Ajoutez cette ligne pour acc�der aux classes de ML-Agents
using Unity.MLAgents.Sensors;  // Pour la collecte d'observations


public class SuperPlayer : Agent
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Surchargez les m�thodes n�cessaires de la classe Agent

    public override void OnEpisodeBegin()
    {
        // Cette m�thode est appel�e au d�but de chaque �pisode, r�initialisez ici l'�tat de votre agent et/ou de l'environnement
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Utilisez cette m�thode pour ajouter les observations que votre agent recueille
        // Par exemple: sensor.AddObservation(someValue);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Utilisez cette m�thode pour d�finir la logique lorsque votre agent prend une action
        // vectorAction contient les actions choisies par la politique de votre agent (ou par le cerveau)
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Si vous voulez tester manuellement votre agent sans le former, vous pouvez d�finir des actions ici
    }
}
