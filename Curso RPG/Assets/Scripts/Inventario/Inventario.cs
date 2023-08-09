using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : Singleton<Inventario>
    {
    
    [SerializeField] private int numeroDeSlots;
    public int NumeroDeSlots =>  numeroDeSlots;

    [Header("Items")]
    [SerializeField] private InventarioItem[] itemsInventario;
    private void Start(){
        itemsInventario = new InventarioItem[numeroDeSlots];
    }

    public void AñadirItem(InventarioItem ItemPorAñadir, int cantidad)
    {
        if (ItemPorAñadir == null) return;

        List<int> indexes = VerificarExistencias(ItemPorAñadir.ID);
   
        if(ItemPorAñadir.EsAcumulable){
            if(indexes.Count > 0){
                for(int i = 0 ; i < indexes.Count; i++)
                {
                    if(itemsInventario[indexes[i]].Cantidad < ItemPorAñadir.AcumulacionMax ){

                        itemsInventario[indexes[i]].Cantidad += cantidad;
                        if( itemsInventario[indexes[i]].Cantidad >ItemPorAñadir.AcumulacionMax)
                        {
                            int diferencia = itemsInventario[indexes[i]].Cantidad - ItemPorAñadir.AcumulacionMax;
                            itemsInventario[indexes[i]].Cantidad = ItemPorAñadir.AcumulacionMax;
                            AñadirItem(ItemPorAñadir,diferencia);
                        }   
                    }
                }
            }
        }
        if(cantidad <= 0){
            return;
        } 

        if(cantidad > ItemPorAñadir.AcumulacionMax){
            AñadirItemEnSlotDisponible(ItemPorAñadir,ItemPorAñadir.AcumulacionMax);
            cantidad -= ItemPorAñadir.AcumulacionMax;
            AñadirItem(ItemPorAñadir, cantidad);
        }
        else{
            AñadirItemEnSlotDisponible(ItemPorAñadir,cantidad);
        }

    }

    private List<int> VerificarExistencias(string ItemID)
    {
        List<int> indices = new List<int>();
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            
            if (itemsInventario[i] != null && itemsInventario[i].ID == ItemID)
            {
                indices.Add(i);
            }
        }
        return indices;
    }

    private void AñadirItemEnSlotDisponible(InventarioItem item, int cantidad) {
        for (int i = 0; i < itemsInventario.Length; i++)
        {
            if (itemsInventario[i] == null)
            {
                itemsInventario[i] = item;
                itemsInventario[i].Cantidad = cantidad;
                break;
            }
        }
    }


}
