using UnityEngine;

public class AquaConsoleTester : MonoBehaviour
{
    public AquaTempFault temp;
    public AquaNutrientFault nutrient;
    public AquaInvasionFault invasion;

    private void Start()
    {
        temp.Activate();
        nutrient.Activate();
        invasion.Activate();

        Debug.Log("AquaConsoleTester ready.");
        Debug.Log("Press: [Q]=cooling, [W]=reduce nutrients, [A]=fix runoff, [E]=remove invasive, [Space]=print status");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) temp.ApplyCooling(1);
        if (Input.GetKeyDown(KeyCode.W)) nutrient.ReduceNutrients(1);
        if (Input.GetKeyDown(KeyCode.A)) nutrient.FixRunoffSource();
        if (Input.GetKeyDown(KeyCode.E)) invasion.RemoveInvasive(1);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(temp.StatusString());
            Debug.Log(nutrient.StatusString());
            Debug.Log(invasion.StatusString());
        }
    }
}