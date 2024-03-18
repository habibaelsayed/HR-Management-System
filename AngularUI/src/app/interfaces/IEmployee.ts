import { Time } from '@angular/common';

export interface IEmployee {
  id: number;
  ssn: number;
  fullName: string;
  address: string;
  phoneNumber: string;
  gender: number;
  nationality: string;
  birthDate: Date;
  contractDate: Date;
  baseSalary: number;
  arrival: Time;
  departure: Time;
  deptId?: number;
  departmentName: string;
}

export enum Gender {
  Male = 'Male',
  Female = 'Female',
}
