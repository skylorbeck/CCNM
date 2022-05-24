using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class AbilityWheel : MonoBehaviour
{
    [SerializeField] private PlayerObject player;
    [SerializeField] private Symbol symbolPrefab;
    private ObjectPool<Symbol> symbolPool;
    [SerializeField] private List<Symbol> symbols = new List<Symbol>();
    [SerializeField] private int totalSymbolsPassed = 0;

    [field:SerializeField] public bool isSpinning { get; private set; } = false;
    [field:SerializeField] public bool winnerChosen { get; private set; } = false;
    [SerializeField] private bool dirty = false;
    [SerializeField] private float spinSpeed = 5f;
    [SerializeField] private float spinSpeedMin = 10f;
    [SerializeField] private float spinSpeedMax = 20f;
    [SerializeField] private int totalToSpin = 20;
    [SerializeField] private int totalToSpinMin = 15;
    [SerializeField] private int totalToSpinMax = 25;
    public Symbol winner { get; private set; }

    void Start()
    {
        symbolPool = new ObjectPool<Symbol>(
            () =>
            {
                Symbol symbol = Instantiate(symbolPrefab, transform);
                symbols.Add(symbol);
                return symbol;
            },
            symbol =>
            {
                symbol.transform.localPosition = new Vector3(0, 3f, 0);
                symbol.SetAbility(player.GetRandomAbility());
                symbol.gameObject.SetActive(true);
            },
            symbol =>
            {
                totalSymbolsPassed++;
                symbol.gameObject.SetActive(false);
            },
            symbol => {
            symbols.Remove(symbol);
            Destroy(symbol);
            },
            true, 10, 10
            );
     
    }

    void Update()
    {
        if (isSpinning)
        {
            foreach (Symbol symbol in symbols)
            {
                if (symbol.isActiveAndEnabled)
                {
                    Transform symbolTransform = symbol.transform;
                    symbolTransform.localPosition = new Vector3(0, symbolTransform.localPosition.y-(spinSpeed*Time.deltaTime), 0);
                    if (symbolTransform.localPosition.y <= -3f)
                    {
                        symbolPool.Release(symbol);
                    }
                }
            }

            if (totalSymbolsPassed > totalToSpin-4)
            {
                isSpinning = false;
                //potentially move this to isSlowing
                
                foreach (Symbol symbol in symbols)
                {
                    if (symbol.isActiveAndEnabled)
                    {
                        Transform symbolTransform = symbol.transform;
                        symbolTransform.localPosition = new Vector3(0, (float)(Math.Round(symbolTransform.localPosition.y/1.5f,MidpointRounding.AwayFromZero)*1.5), 0);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    public async void Spin()
    {
        winner = null;
        winnerChosen = false;
        totalSymbolsPassed = 0 - symbolPool.CountActive;
        isSpinning = true;
        spinSpeed = Random.Range(spinSpeedMin, spinSpeedMax);
        totalToSpin = Random.Range(totalToSpinMin, totalToSpinMax);

        do
        {
            dirty = false;
            for (var i = 0; i < symbols.Count; i++)
            {
                if (symbols[i].isActiveAndEnabled && symbols[i].transform.localPosition.y>=1.85f)
                {
                    dirty = true;
                }
            }
            await Task.Delay(10);
        } while (dirty);
        

        for (int i = 0; i < totalToSpin ; i++)
        {
            Symbol symbol = symbolPool.Get();
            do
            {
                await Task.Delay(10);
            } while (symbol.transform.localPosition.y>1.6f);
          
        }
    }

    public Symbol GetWinner()
    {
        winnerChosen = true;
        winner = null;
        foreach (Symbol symbol in symbols)
        {
            if (symbol.isActiveAndEnabled)
            {
                Transform symbolTransform = symbol.transform;
                if(symbolTransform.localPosition.y<=0.1f && symbolTransform.localPosition.y>=-0.1f)
                {
                    winner = symbol;
                    break;
                }
            }
        }
        return winner;
    }
}
