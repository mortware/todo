import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { catchError, filter, map, Observable, throwError } from 'rxjs';
import { TodoService } from 'src/app/core/services/todo.service';
import { BaseComponent } from '../base.component';
import { faCheck } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.scss']
})
export class TodoListComponent extends BaseComponent {

  vm$: Observable<any>;
  form: FormGroup;
  faCheck = faCheck;  
  toggle : boolean = false  

  constructor(
    private todoService: TodoService,
    private formBuilder: FormBuilder) {
    super();

    this.form = this.formBuilder.group({
      text: new FormControl(null, Validators.required)
    })

    this.vm$ = this.todoService.list$.pipe(
      map(items => {
        return {
          items
        };
      }),
      catchError(err => {
        this.handleError('Failed to fetch items', err.message);
        return throwError(() => err);
      })
    );    
    
  }
  
  markComplete(item : any){    
    this.todoService.update(item.id).subscribe();
  }

  submit() {
    this.todoService.create(this.form.get('text')?.value)
      .subscribe(() => this.form.reset());
  }
}
