using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventarioUI : MonoBehaviour
{
    [SerializeField] private InventarioSlot slotPrefab;

    [SerializeField] private Transform contenedor;


    List<InventarioSlot> slotsDisponibles = new List<InventarioSlot>();

    private void Start()
    {
        InicializarInventario();
    
    }

    private void InicializarInventario()
    {
        for(int i = 0; i < Inventario.Instance.NumeroDeSlots; i++)
        {
            InventarioSlot nuevoSlot  =  Instantiate(slotPrefab,contenedor);
            nuevoSlot.Index = i;
            slotsDisponibles.Add(nuevoSlot);
        } 
    }
}
