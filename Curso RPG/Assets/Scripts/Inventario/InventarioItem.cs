using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TiposDeItem
{
    Armas,
    Pociones,
    Pergaminos,
    Ingredientes,
    Tesoros
}
public class InventarioItem : ScriptableObject
{
    [Header("Parametros")]
    public string ID;
    public string Nombre;
    public Sprite Icono;
    
    [TextArea] public string Description;

    [Header("Informacion")]
    public TiposDeItem Tipo;
    public bool EsConsumible;
    public bool EsAcumulable;
    public int AcumulacionMax;
    public int Cantidad;
    
}