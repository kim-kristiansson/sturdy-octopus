using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject fixturePrefab;
    public GameObject basketPrefab;
    private GameObject currentObject;

    void Start()
    {
        CreateFixture();
    }

    public void CreateFixture()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        currentObject = Instantiate(fixturePrefab, transform.position, Quaternion.identity);
    }

    public void SwitchToBasket()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }
        currentObject = Instantiate(basketPrefab, transform.position, Quaternion.identity);
    }
}
