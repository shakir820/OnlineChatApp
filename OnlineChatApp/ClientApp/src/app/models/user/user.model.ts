
export class User {

  id: number;
  first_name: string;
  last_name: string;
  email: string;
  connection_id: string;

  get name(): string {
    return this.first_name + ' ' + this.last_name;
  }

 }
