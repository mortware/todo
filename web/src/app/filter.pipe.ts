import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {
transform(array: any, field: boolean): any[] {
  console.log(field);
  console.log(field);
  if(!field){
    var items = array.filter(function(item : any) {            
      return item.completed == undefined
    });    
    console.log(items)
    return items;
  }
    return array;    
  }
}