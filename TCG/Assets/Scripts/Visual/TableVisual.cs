using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class TableVisual : MonoBehaviour 
{
    public AreaPosition owner;

    // ссылка на объект, который отмечает позиции, на которые необходимо помещать существ
    public SameDistanceChildren slots;

    public List<GameObject> CreaturesOnTable = new List<GameObject>();

    // затрагивает ли курсор коллайдер col
    private bool cursorOverThisTable = false;
    private BoxCollider col;

    // затрагивает ли курсор один из table`ов
    public static bool CursorOverSomeTable
    {
        get
        {
            TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
            return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
        }
    }

    // затрагивает ли курсор этот table
    public bool CursorOverThisTable
    {
        get{ return cursorOverThisTable; }
    }

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    void Update()
    {
        // использую raycast т.к. OnMouseEnter реагирует на коллайдеры самих карт, а карты находятся над столом
        // лучи направляются от камеры к курсору
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

        bool passedThroughTableCollider = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughTableCollider = true;
        }
        cursorOverThisTable = passedThroughTableCollider;
    }
   
    public void AddCreatureAtIndex(CardAsset ca, int UniqueID, int index)
    {
        // создаем новый gameobject
        GameObject creature = GameObject.Instantiate(GlobalSettings.Instance.CreaturePrefab, slots.Children[index].transform.position, Quaternion.identity) as GameObject;

        // настраиваем внешний вид карты из ассета
        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();

        // добавляем тэг в соответствии с владельцем карты
        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString()+"Creature";
        
        // привязываем созданный объект к слотам на столе
        creature.transform.SetParent(slots.transform);

        // добавляем в список (на определенное место)
        CreaturesOnTable.Insert(index, creature);

        // позволяем созданному объекту знать свое местоположение
        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowTable;
        else
            w.VisualState = VisualStates.TopTable;

        // добавляем уникальный ID
        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // обновляем местоположение остальных существ на столе
        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();

        //TEST

        DraggableBattlecry DB = creature.gameObject.GetComponentInChildren<DraggableBattlecry>();
        if (DB != null)
            DB.ActivateDragging();

        // заканчиваем выполнение комманды
        Command.CommandExecutionComplete();
    }


    // определяет индекс для размещения нового существа в зависимости от координаты X курсора
    public int TablePosForNewCreature(float MouseX)
    {
        // если на столе нет существ или если мы размещаем существо справа от всех остальных
        // справа, т.к. слоты пронумерованы справа налево от 0 до 4
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Children[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Children[CreaturesOnTable.Count - 1].transform.position.x) // курсор левее всех остальных существ на столе
            return CreaturesOnTable.Count;
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.Children[i].transform.position.x && MouseX > slots.Children[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }

    // уничтожение существа
    public void RemoveCreatureWithID(int IDToRemove)
    {
        // TODO: This has to last for some time
        // Adding delay here did not work because it shows one creature die, then another creature die. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {
                
        //    });
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Command.CommandExecutionComplete();
    }

    // меняем положение слотов на столе в зависимости от количества существ на столе
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CreaturesOnTable.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    // меняем расположение существ на столе после добавления нового / смерти существующего
    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CreaturesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Children[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }

}
