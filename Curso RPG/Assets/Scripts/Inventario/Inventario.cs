using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventario : Singleton<Inventario>
{
    [Header("Items")] 
    [SerializeField] private InventarioItem[] itemsInventario;
    [SerializeField] private Personaje personaje;
    [SerializeField] private int numeroDeSlots;

    public Personaje Personaje => personaje;
    public int NumeroDeSlots => numeroDeSlots;
    public InventarioItem[] ItemsInventario => itemsInventario;

    private void Start()
    {
        itemsInventario = new InventarioItem[numeroDeSlots];
    }

    public void AñadirItem(InventarioItem itemPorAñadir, int cantidad)
    {
        if (itemPorAñadir == null)
        {
            return;
        }

        // Verificacion en caso tener ya un item similar en inventario
        List<int> indexes = VerificarExistencias(itemPorAñadir.ID);
        if (itemPorAñadir.EsAcumulable)
        {
            if (indexes.Count > 0)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    if (itemsInventario[indexes[i]].Cantidad < itemPorAñadir.AcumulacionMax)
                    {
                        itemsInventario[indexes[i]].Cantidad += cantidad;
                        if (itemsInventario[indexes[i]].Cantidad > itemPorAñadir.AcumulacionMax)
                        {
                            int diferencia = itemsInventario[indexes[i]].Cantidad - itemPorAñadir.AcumulacionMax;
                            itemsInventario[indexes[i]].Cantidad = itemPorAñadir.AcumulacionMax;
                            AñadirItem(itemPorAñadir, diferencia);
                        }
                        
                        InventarioUI.Instance.DibujarItemEnInventario(itemPorAñadir, 
                            itemsInventario[indexes[i]].Cantidad, indexes[i]);
                        return;
                    }
                }
            }
        }

        if (cantidad <= 0)
        {
            return;
        }

        if (cantidad > itemPorAñadir.AcumulacionMax)
        {
            AñadirItemEnSlotDisponible(itemPorAñadir, itemPorAñadir.AcumulacionMax);
            cantidad -= itemPorAñadir.AcumulacionMax;
            AñadirItem(itemPorAñadir, cantidad);
        }
        else
        {
            AñadirItemEnSlotDisponible(itemPorAñadir, cantidad);
        }
    }

    private List<int> VerificarExistencias(string itemID)
    {
        List<int> indexesDelItem = new List<int>();
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] != null)
            {
                if (itemsInventario[i].ID == itemID) 
                {
                    indexesDelItem.Add(i);
                }
            }
        }

        return indexesDelItem;
    }

    private void AñadirItemEnSlotDisponible(InventarioItem item, int cantidad)
    {
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] == null)
            {
                itemsInventario[i] = item.CopiarItem();
                itemsInventario[i].Cantidad = cantidad;
                InventarioUI.Instance.DibujarItemEnInventario(item, cantidad, i);
                return;
            }
        }
    }

    private void EliminarItem(int index)
    {
        ItemsInventario[index].Cantidad--;
        if (itemsInventario[index].Cantidad <= 0)
        {
            itemsInventario[index].Cantidad = 0;
            itemsInventario[index] = null;
            InventarioUI.Instance.DibujarItemEnInventario(null, 0, index);
        }
        else
        {
            InventarioUI.Instance.DibujarItemEnInventario(itemsInventario[index], 
                itemsInventario[index].Cantidad, index);
        }
    }

    public void MoverItem(int indexInicial, int indexFinal)
    {
        if (itemsInventario[indexInicial] == null || itemsInventario[indexFinal] != null)
        {
            return;
        }
        
        // Copiar item en slot final
        InventarioItem itemPorMover = itemsInventario[indexInicial].CopiarItem();
        itemsInventario[indexFinal] = itemPorMover;
        InventarioUI.Instance.DibujarItemEnInventario(itemPorMover, itemPorMover.Cantidad, indexFinal);
        
        // Borramos Item de Slot inicial
        itemsInventario[indexInicial] = null;
        InventarioUI.Instance.DibujarItemEnInventario(null, 0, indexInicial);
    }
    
    private void UsarItem(int index)
    {
        if (itemsInventario[index] == null)
        {
            return;
        }

        if (itemsInventario[index].UsarItem())
        {
            EliminarItem(index);
        }
    }
    
    #region Eventos

    private void SlotInteraccionRespuesta(TipoDeInteraccion tipo, int index)
    {
        switch (tipo)
        {
            case TipoDeInteraccion.Usar:
                UsarItem(index);
                break;
            case TipoDeInteraccion.Equipar:
                break;
            case TipoDeInteraccion.Remover:
                break;
        }
    }
    
    private void OnEnable()
    {
        InventarioSlot.EventoSlotInteraccion += SlotInteraccionRespuesta;
    }

    private void OnDisable()
    {
        InventarioSlot.EventoSlotInteraccion -= SlotInteraccionRespuesta;
    }

    #endregion
}
