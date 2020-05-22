using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IEntity
{
    public int ID;
    public string Username;
    public float[] Colour;

    public int Level { get; private set; } = 0;

    public ClassType Class { get; private set; }

    public Inventory Inventory;

    private float moveSpeed = 5f / Constants.TICKS_PER_SECOND;
    private bool[] inputs;

    public bool EffectImmune { get; } = false;
    public bool DamageImmune { get; } = false;

    public Health Health { get; private set; }
    public Resource Barrier { get; private set; }
    public Resource Resource { get; private set; }

    public Statistic AttackPower { get; private set; }
    public Statistic Spite { get; private set; }
    public Statistic Rigor { get; private set; }
    public Statistic Toughness { get; private set; }
    public Statistic Constitution { get; private set; }
    public Statistic Haste { get; private set; }

    public void Initialize(int _id, PlayerSave _data)
    {
        ID = _id;

        Username = _data.Name;
        Level = _data.Level;
        Class = (ClassType)_data.CharacterClass;
        Colour = new float[3] { _data.Color.r, _data.Color.g, _data.Color.b };

        inputs = new bool[4];

        Inventory = new Inventory(this);

        AttackPower = new Statistic();
        Spite = new Statistic();
        Rigor = new Statistic();
        Toughness = new Statistic();
        Constitution = new Statistic();
        Haste = new Statistic();

        Health = new Health(Class, Level, ClassConstants.HPPerLevel, Toughness, Constitution);
        Barrier = new Resource(0);
        Resource = new Resource(Class, Level, ClassConstants.ResourcePerLevel);
    }

    public void FixedUpdate()
    {
        Vector2 inputHeading = Vector2.zero;

        if (inputs[0])
            inputHeading.y++;
        if (inputs[1])
            inputHeading.y--;
        if (inputs[2])
            inputHeading.x--;
        if (inputs[3])
            inputHeading.x++;

        Move(inputHeading);
    }

    public void SetInput(bool[] _inputs)
    {
        inputs = _inputs;
    }
    public void TryInteract()
    {
        RaycastHit[] hits = Physics.SphereCastAll(new Ray(), 1.5f, 1.5f, LayerMask.NameToLayer("ItemDrops"));

        float distFromPlayer = 2f;
        int ClosestByIndex = -1;

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].distance <= distFromPlayer)
            {
                distFromPlayer = hits[i].distance;
                ClosestByIndex = i;
            }
        }

        if(ClosestByIndex != -1)
        {
            hits[ClosestByIndex].collider.GetComponent<ItemDrop>().PickUp(this);
        }
    }

    private void Move(Vector2 _heading)
    {
        transform.position += new Vector3(_heading.x, _heading.y) * moveSpeed;
        ServerSend.PlayerPosition(this);
    }



    public IRDSObject CreateInstance()
    {
        return null;
    }
}
