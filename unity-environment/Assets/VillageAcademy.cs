using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Jobs { Unemployed, Farmer, Miner }

public class VillageAcademy : Academy {

    [SerializeField]
    List<House> houses = new List<House>();
    public void AddHouse(House house) { houses.Add(house); }
    public void RemoveHouse(House house) { houses.Remove(house); }

    [SerializeField]
    List<Factory> farms;
    public void AddFarm(Factory farm) { farms.Add(farm); }
    public void RemoveFarm(Factory farm) { farms.Remove(farm); }
    float foodProd;
    [SerializeField]
    float food = 50;

    [SerializeField]
    List<Factory> coalPlants = new List<Factory>();
    public void AddCoalPlant(Factory coalPlant) { coalPlants.Add(coalPlant); }
    public void RemoveCoalPlant(Factory coalPlant) { coalPlants.Remove(coalPlant); }
    float coalProd;
    [SerializeField]
    float coal = 50;

    [SerializeField]
    List<Bar> bars = new List<Bar>();
    public void AddBar(Bar bar) { bars.Add(bar); }
    public void RemoveBar(Bar bar) { bars.Remove(bar); }
    [SerializeField]
    float happiness = 50;
    [SerializeField]
    GameObject villagerPrefab;
    [SerializeField]
    [Range(0, 100)]
    float babyProgress = 0f;

    [Header("UI")]
    [SerializeField]
    Image coalUI;
    [SerializeField]
    Image foodUI;
    [SerializeField]
    Image happinessUI;
    [SerializeField]
    Image reproducitonUI;
    [SerializeField]
    Text population;
    [SerializeField]
    Slider timeUI;

    public static Queue<GameObject> jobOffers = new Queue<GameObject>();

	public override void InitializeAcademy()
    {

    }

	public override void AcademyReset()
    {

    }

    public override void AcademyStep()
    {
        // Update productions
        foodProd = -VillagerAgent.AgentCount * 0.2f;

        foreach (Factory f in farms) {
            foodProd += f.GetProd();
        }

        food += foodProd;
        food = Mathf.Clamp(food, -100, 500);        

        coalProd = 0;

        foreach (Factory f in coalPlants) {
            coalProd += f.GetProd();
        }

        coalProd -= houses.Count * 0.2f;

        coal += coalProd;
        coal = Mathf.Clamp(coal, -100, 500);

        happiness = VillagerAgent.AvgHappiness;

        foreach (House house in houses) {
            if (house.IsFull)
                continue;
            
            babyProgress += happiness > 50 ? (happiness-40) /10 : 0;

            if (babyProgress >= 100f || (VillagerAgent.AgentCount == 0 && houses.Count > 0 && farms.Count > 0 && bars.Count > 0 && coalPlants.Count > 0)) {
                babyProgress = 0f;

                GameObject villager = Instantiate(villagerPrefab);
                VillagerAgent va = villager.GetComponent<VillagerAgent>();

                house.AddOwner(va);

                va.Bar = bars[Random.Range(0, bars.Count)].gameObject;

                va.GiveBrain(GetComponentInChildren<Brain>());
            }
            break;
        }

        GameObject farm = farms[Random.Range(0, farms.Count)].gameObject;
        if (Random.Range(food - 250f, food) < 250f && !jobOffers.Contains(farm)) {
            jobOffers.Enqueue(farm);
        }

        GameObject c = coalPlants[Random.Range(0, coalPlants.Count)].gameObject;
        if (Random.Range(coal - 250f, coal) < 250f && !jobOffers.Contains(c)) {
            jobOffers.Enqueue(c);
        }
    }

    void Update() {
        coalUI.fillAmount = Mathf.Lerp(coalUI.fillAmount, (coal + 100) / 600f, Time.deltaTime);
        coalUI.color = new Color(-coal/100, coal/500, 0);
        
        foodUI.fillAmount = Mathf.Lerp(foodUI.fillAmount, (food + 100) / 600f, Time.deltaTime);
        foodUI.color = new Color(-food/100, food/500, 0);
        
        happinessUI.fillAmount = Mathf.Lerp(happinessUI.fillAmount, happiness / 100f, Time.deltaTime);
        happinessUI.color = new Color(1, happiness / 100, 0);        
        
        reproducitonUI.fillAmount = Mathf.Lerp(reproducitonUI.fillAmount, babyProgress / 100f, Time.deltaTime);

        if (houses.Count > 0) {
            StringBuilder sb = new StringBuilder();
            sb.Append("population: ");
            sb.Append(VillagerAgent.AgentCount.ToString());
            sb.Append("/");
            sb.Append(houses.Count * houses[0].capacity);
            population.text = sb.ToString();
        }

        Time.timeScale = timeUI.value;
    }
}
