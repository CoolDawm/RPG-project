using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public int power;

    public override string ToString()
    {
        return abilityName;
    }
}
