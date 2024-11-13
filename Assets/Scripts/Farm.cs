using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class Farm : MonoBehaviour
{
    public readonly Dictionary<string, List<string>> Colors = new()
    {
        {"Black",  new() {"Black", "Black"} },
        {"Blue", new () {"Blue", "Blue"} },
        {"Red", new() { "Red", "Red" } },
        {"Yellow", new() { "Yellow", "Yellow" } }
    };
    public GameObject plotPrefab;
    private GameObject plotSelected;
    public GameObject seedSelectUI;
    public GameObject catsulePrefab;

    
    public GameObject catSelectUI;
    public GameObject plotsParent;
    private bool isSeedSelectUI = false;
    private bool isCatSelectUI = false;
    public GameObject catsuleselected;
    public GameObject seedselected;


    private Vector3 mousePos;
    private bool plotMode = true; //This is just until we implement a proper "I want to plow." mechanic.

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
                HandleClick();
        }
        else if (Input.GetMouseButtonDown(1))
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(1))
        {
            if (mousePos != Vector3.zero && Camera.main.ScreenToWorldPoint(Input.mousePosition) != mousePos)
                Camera.main.transform.position -= Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePos;
        }
        else if (Input.GetMouseButtonUp(1))
            mousePos = Vector3.zero;

    }
    private void HandleClick()
    {
        Debug.Log("Mouse Up!");
        if (!seedSelectUI.activeSelf && !catSelectUI.activeSelf)
        {
            Vector3 db = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<GameObject> collidedGameObjects = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0)
                    .Select(c => c.gameObject).Where(x => !x.name.Contains("(9x9)")).ToList();
            GameObject land = collidedGameObjects.Find(c => c.name == "Land");
            collidedGameObjects.Remove(land);
            GameObject plot = collidedGameObjects.Find(c => c.CompareTag("Plot")); //Im thinking do the planting of seeds the same way as this, where I look for the Plot name and then it pops up the UI element to plant a seed.
            collidedGameObjects.Remove(plot);
            if (land && plotMode)
            {
                Vector3 pos = land.transform.position;
                GameObject newPlot = Instantiate(plotPrefab, pos, plotPrefab.transform.rotation);
                newPlot.transform.SetParent(GameObject.Find("Plots").transform);
                //plotSelected = newPlot;
                //OpenUI();
                land.SetActive(false);
                Debug.Log("Plot placed!");
            }
            else if (collidedGameObjects.Count == 0 && plot)
            {
                plotSelected = plot;
                OpenSeedUI();
            }
            collidedGameObjects.Clear();
        }
        else if (IsClickOverUI(seedselected) && plotSelected)
            OpenSeedUI();
        else if (IsClickOverUI(catsuleselected) && plotSelected)
            OpenCatUI();
    }
    public void PlantSeed(PlantData plantData)
    {
        if (plotSelected != null)
        {
            plotSelected.GetComponent<Plot>().Plant(plantData);
            plotSelected = null;
        }
        //StartCoroutine(DelayMenu(seedSelectUI));
        CloseSeedUI();
        //plotSelected.GetComponent<Collider2D>().enabled = true;
    }
    public void PlantCatsule()
    {
        if (plotSelected != null)
        {
            GameObject catsule = Instantiate(catsulePrefab, plotSelected.transform.position, plotSelected.transform.rotation);
            plotSelected.GetComponent<Plot>().Plant(catsule.GetComponent<Catsule>());
            //plotSelected.GetComponent<Collider2D>().enabled = true;
            plotSelected = null;
        }
        //StartCoroutine(DelayMenu(catSelectUI));
        CloseCatUI();
        //plotSelected.GetComponent<Collider2D>().enabled = true;
    }

    public void OpenSeedUI()
    {
        if (plotSelected == null) return;
        if (isCatSelectUI) CloseCatUI();
        seedselected.SetActive(true);

        seedSelectUI.SetActive(true);  
        isSeedSelectUI = true;

        Collider2D[] plotColliders = plotsParent.GetComponentsInChildren<Collider2D>();

        if (plotColliders.Length == 0)
        {
            Debug.LogWarning("No Collider2D components found in child objects of plotsParent.");
        }

        //foreach (Collider2D collider in plotColliders)
        //{
        //    if (collider.gameObject.CompareTag("Plot")) 
        //    { 
        //        collider.enabled = false;
        //    }
        //}

        Debug.Log("seedselect open");
    }
    public void CloseSeedUI()
    {
        seedselected.SetActive(false);
        isSeedSelectUI = false;
        StartCoroutine(DelayMenu(seedSelectUI));
        Debug.Log("zsxdfcgvbhjnkml,;.'xetcfyvgubhnjmk,l");

        Collider2D[] plotColliders = plotsParent.GetComponentsInChildren<Collider2D>();

        if (plotColliders.Length == 0)
        {
            Debug.LogWarning("No Collider2D components found in child objects of plotsParent.");
        }
        //foreach (Collider2D collider in plotColliders)
        //{
        //    if (collider.gameObject.CompareTag("Plot"))
        //    {
        //        collider.enabled = true;
        //    }
        //}
    }
    public void OpenCatUI()
    {
        if (!plotSelected) return;
        if(isSeedSelectUI) CloseSeedUI();
        catsuleselected.SetActive(true);

        catSelectUI.SetActive(true);
        isCatSelectUI = true;

        Collider2D[] plotColliders = plotsParent.GetComponentsInChildren<Collider2D>();

        if (plotColliders.Length == 0)
        {
            Debug.LogWarning("No Collider2D components found in child objects of plotsParent.");
        }

        //foreach (Collider2D collider in plotColliders)
        //{
        //    if (collider.gameObject.CompareTag("Plot"))
        //    {
        //        collider.enabled = false;
        //    }
        //}

        Debug.Log("seedselect open");
    }
    public void CloseCatUI()
    {
        catsuleselected.SetActive(false);
        isCatSelectUI = false;
        StartCoroutine(DelayMenu(catSelectUI));

        Debug.Log("zsxdfcgvbhjnkml,;.'xetcfyvgubhnjmk,l");
        Collider2D[] plotColliders = plotsParent.GetComponentsInChildren<Collider2D>();

        if (plotColliders.Length == 0)
        {
            Debug.LogWarning("No Collider2D components found in child objects of plotsParent.");
        }

        //foreach (Collider2D collider in plotColliders)
        //{
        //    if (collider.gameObject.CompareTag("Plot"))
        //    {
        //        collider.enabled = true;
        //    }
        //}
    }
    private bool IsClickOverUI(GameObject uiElement)
    {

        RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition,
            Camera.main);

    }

    public void SeedSelectEnter()
    {
        seedselected.SetActive(true);
    }
    public void CatsuleSelectEnter()
    {
        catsuleselected.SetActive(true);
    }
    public void SeedSelectExit()
    {
        if (isSeedSelectUI) { return; }
        seedselected.SetActive(false);
    }
    public void CatsuleSelectExit()
    {
        if (isCatSelectUI) { return; }
        catsuleselected.SetActive(false);
    }
    IEnumerator DelayMenu(GameObject UI)
    {
        yield return new WaitForSeconds(.25f);
        UI.SetActive(false);
    }
}