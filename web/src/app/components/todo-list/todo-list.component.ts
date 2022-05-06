import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { catchError, map, Observable, throwError } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck } from '@fortawesome/free-solid-svg-icons';
import { TodoItem } from 'src/app/core/models';
import { faSquare } from '@fortawesome/free-regular-svg-icons';
@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;
  farSquare = faSquare;
  _submitting : Boolean = false;
  _showCompleted:Boolean = false;

  constructor(
    private todoService: TodoService,
    private formBuilder: FormBuilder) {
    super();

    this.form = this.formBuilder.group({
      text: new FormControl(null)
    })
    

    this.vm$ = this.todoService.list$.pipe(
      map(items => {
        var sorted = items.sort((a,b)=>Date.parse(b.created.toString())- Date.parse(a.created.toString()))
        return {items:sorted};
      }),
      catchError(err => {
        this.handleError('Failed to fetch items', err.message);
        return throwError(() => err);
      })
    );
  }
  submit() {
    this._submitting = true;
    this.todoService.create(this.form.get('text')?.value)
      .subscribe(() => {
        this.form.reset();
        this._submitting = false;
      });
  }

  complete(item:TodoItem){
    if(item.completed)
      return;

    item.completed = new Date();
    this.todoService.update(item).subscribe(()=>{});
  }


  

}
