using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageAcademy : Academy {

    [SerializeField]
    GameObject[] houses;

    [SerializeField]
    Factory[] farms;
    float foodProd;
    [SerializeField]
    float food = 50;

    [SerializeField]
    Factory[] coalPlants;
    float coalProd;
    [SerializeField]
    float coal = 50;

    [SerializeField]
    Bar[] bars;
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
    Slider timeUI;

	public override void InitializeAcademy()
    {

    }

	public override void AcademyReset()
    {

    }

    public override void AcademyStep()
    {
        
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

        coalProd -= houses.Length * 0.2f;

        coal += coalProd;
        coal = Mathf.Clamp(coal, -100, 500);

        happiness = VillagerAgent.AvgHappiness;

        foreach (GameObject house in houses) {
            if (house.GetComponent<House>().IsFull)
                continue;
            
            babyProgress += happiness > 50 ? (happiness-40) /10 : 0;

            if (babyProgress >= 100f) {
                babyProgress = 0f;

                GameObject villager = Instantiate(villagerPrefab);
                VillagerAgent va = villager.GetComponent<VillagerAgent>();

                house.GetComponent<House>().AddOwner(va);

                if (coal != food && coal < food) {
                    va.Work = coalPlants[Random.Range(0, coalPlants.Length)].gameObject;
                } else if (coal != food) {
                    va.Work = farms[Random.Range(0, farms.Length)].gameObject;
                } else if (Random.Range(0, 2) == 0) {
                    va.Work = coalPlants[Random.Range(0, coalPlants.Length)].gameObject;                    
                } else {
                    va.Work = farms[Random.Range(0, farms.Length)].gameObject;                    
                }

                va.Bar = bars[Random.Range(0, bars.Length)].gameObject;

                va.GiveBrain(GetComponentInChildren<Brain>());
            }
            break;
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

        Time.timeScale = timeUI.value;
    }
}
