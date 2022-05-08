import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'orderBy'
})
export class OrderByPipe implements PipeTransform {
transform(array: any, field: string, sortType? : string): any[] {  
  array.sort((a: any, b: any) => {
    if (a[field] < b[field]) {
      return -1;
    } else if (a[field] > b[field]) {
      return 1;
    } else {
      return 0;
    }
  });
  
  if(sortType == null)
    sortType == "ASC";
    
  return sortType == "ASC" ? array : array.reverse()   
}
}