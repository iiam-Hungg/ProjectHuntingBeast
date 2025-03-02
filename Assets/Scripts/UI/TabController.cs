using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActiveTab(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveTab(int tabNo)
    {
        for (int i = 0; i < pages.Length; i++) 
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        pages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
    }
}
