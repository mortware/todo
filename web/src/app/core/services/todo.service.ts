import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { TodoItem } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TodoService {

  static readonly BasePath = 'https://localhost:7050/todo';

  private refreshList$ = new BehaviorSubject<boolean>(true);

  list$: Observable<TodoItem[]>;

  constructor(private http: HttpClient) {

    this.list$ = this.list();

  }

  private list(): Observable<TodoItem[]> {
    return this.refreshList$.pipe(switchMap(
      _ => this.http.get<TodoItem[]>(TodoService.BasePath + '/list')
    ))
  }

  create(text: string): Observable<string> {
    return this.http.post<string>(TodoService.BasePath + '/create', { text }).pipe(
      tap(_ => {
        this.refreshList$.next(false);
      })
    );
  }

  update(item:TodoItem):Observable<TodoItem>{
    var r =  this.http.post<TodoItem>(TodoService.BasePath+"/update",item).pipe(
      tap(_=>{
        this.refreshList$.next(false);
      })
    )
    return r;
  }

}
