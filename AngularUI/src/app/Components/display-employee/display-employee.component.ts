import { state } from '@angular/animations';
import { Router } from '@angular/router';
import { IEmployee } from 'src/app/interfaces/IEmployee';
import { EmpServicesService } from 'src/app/services/emp-services.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, TemplateRef } from '@angular/core';



@Component({
  selector: 'app-display-employee',
  templateUrl: './display-employee.component.html',
  styleUrls: ['./display-employee.component.css'],
})
export class DisplayEmployeeComponent implements OnInit {
  employees: IEmployee[] = [];
  modalRef?: BsModalRef; // this is a reference to bootstrap modal

  totalLength: any;
  page: number = 1;
  searchText: any;
  deleteemp: number = 1;

  constructor(
    private employeeService: EmpServicesService,
    private router: Router,
    private modalService: BsModalService,

  ) { }
  ngOnInit(): void {

    this.closeFormDelete();
    this.employeeService.getEmployees().subscribe((data) => {
      this.employees = data as IEmployee[];
    });
  }
  addEmployee() {
    this.router.navigate(['/employee/add']);
  }
  deleteEmployee(id: number): void {
    this.modalRef?.hide();
    console.log(id)
    this.employeeService.deleteEmployee(id).subscribe(() => {
      this.employees = this.employees.filter((emp) => emp.id !== id);
    });

    this.closeFormDelete();
  }
  updateForm(employee: IEmployee) {
    this.router.navigate(['/employee/update'], { state: { employee } });
  }
  trackByFn(index: number, employee: IEmployee) {
    return employee.id;
  }

  closeFormDelete() {
    const popupForm = document.getElementById('popupFormDelete') as HTMLElement;
    popupForm.style.display = 'none';
  }
  SureForm(id: number) {

    this.deleteemp = id;
    const popupFormDelete = document.getElementById('popupFormDelete') as HTMLElement;
    popupFormDelete.style.display = 'block';

  }

  decline(): void {
    this.modalRef?.hide();
  }
  openModal(template: TemplateRef<void>, id: number) {
    this.deleteemp = id;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }
}
