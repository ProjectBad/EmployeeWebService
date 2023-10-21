
# API Definition


## Create Employee

### Create Employee Request

```js
POST {{host}}/api/Employee/Create
```

```json
{
  "name": "Maks",
  "surname": "Romanov",
  "phone": "79374258214",
  "companyid": 2,
  "passport": {
    "type": "PASPORT",
    "number": "788899985"
  },
  "department": {
    "name": "SMARTWAY",
    "phone": "84191"
  }
}
```

### Create Employee Response

```js
200 Ok
```

```json
{
  "id": 1,
  "name": "Maks",
  "surname": "Romanov",
  "phone": "79374258214",
  "companyid": 2,
  "passport": {
    "type": "PASPORT",
    "number": "788899985"
  },
  "department": {
    "name": "SMARTWAY",
    "phone": "84191"
  }
}
```

## Get Employee

### Get Employee Request
```js
GET {{host}}/api/Employee/GetbyId/:id
```

### Get Employee Response

```js
200 Ok
```

```json
{
  "id": 1,
  "name": "Maks",
  "surname": "Romanov",
  "phone": "79374258214",
  "companyid": 2,
  "passport": {
    "type": "PASPORT",
    "number": "788899985"
  },
  "department": {
    "name": "SMARTWAY",
    "phone": "84191"
  }
}
```

### Get Employee Request
```js
GET {{host}}/api/Employee/GetAllByCompanyId/?companyid={id}
```

### Get Employee Response

```js
200 Ok
```

```json
[
    {
        "id": 6,
        "name": "Maks",
        "surname": "MAKSOV",
        "phone": "76786878",
        "companyid": 3,
        "passport": {
            "type": "PASPORT",
            "number": "587868"
        },
        "department": {
            "name": "SMARTWAY",
            "phone": "51231"
        }
    },
    {
        "id": 7,
        "name": "Maks",
        "surname": "MAKSOV",
        "phone": "76786878",
        "companyid": 3,
        "passport": {
            "type": "PASPORT",
            "number": "587868"
        },
        "department": {
            "name": "SMARTA",
            "phone": "51231"
        }
    },
    {
        "id": 8,
        "name": "Maks",
        "surname": "MAKSOV",
        "phone": "76786878",
        "companyid": 3,
        "passport": {
            "type": "PASPORT",
            "number": "587868"
        },
        "department": {
            "name": "SMARTWAY",
            "phone": "51231"
        }
    }
]
```

### Get Employee Request
```js
GET {{host}}/api/Employee/GetAllByCompanyAndDepartment/?companyid={companyid}&departmentname={departmentname}
```

### Get Employee Response

```js
200 Ok
```

```json
[
    {
        "id": 6,
        "name": "Maks",
        "surname": "MAKSOV",
        "phone": "76786878",
        "companyid": 3,
        "passport": {
            "type": "PASPORT",
            "number": "587868"
        },
        "department": {
            "name": "SMARTWAY",
            "phone": "51231"
        }
    },
    {
        "id": 8,
        "name": "Maks",
        "surname": "MAKSOV",
        "phone": "76786878",
        "companyid": 3,
        "passport": {
            "type": "PASPORT",
            "number": "587868"
        },
        "department": {
            "name": "SMARTWAY",
            "phone": "51231"
        }
    }
]
```
## Update Employee

### Update Employee Request

```js
PUT {{host}}/api/Employee/Update
```

```json
{
  "id": "3", 
  "name": "Maks",
  "surname": "Romanov",
  "phone": "79374258214",
  "companyid": 1,
  "passport": {
    "type": "PASPORT",
    "number": "788899985"
  },
  "department": {
    "name": "SMARTWAY",
    "phone": "84191"
  }
}
```
### Update Employee Response

```js
200 Ok
```

```json
{
    "id": 3,
    "name": "Maks",
    "surname": "Romanov",
    "phone": "79374258214",
    "companyid": 1,
    "passport": {
        "type": "PASPORT",
        "number": "788899985"
    },
    "department": {
        "name": "SMARTWAY",
        "phone": "84191"
    }
}
```

### Update Employee Request

```js
PUT {{host}}/api/Employee/Update?id=3
```

```json
{
  "name": "Maks",
  "surname": "Romanov",
  "phone": "79374258214",
  "companyid": 1,
  "passport": {
    "type": "PASPORT",
    "number": "788899985"
  },
  "department": {
    "name": "SMARTWAY",
    "phone": "84191"
  }
}
```
### Update Employee Response

```js
200 Ok
```

```json
{
    "id": 3,
    "name": "Maks",
    "surname": "Romanov",
    "phone": "79374258214",
    "companyid": 1,
    "passport": {
        "type": "PASPORT",
        "number": "788899985"
    },
    "department": {
        "name": "SMARTWAY",
        "phone": "84191"
    }
}
```
## Remove Employee

### Remove Employee Request

```js
DELETE {{host}}/api/Employee/Remove?id={id}
```

### Update Employee Response

```js
200 Ok
```
