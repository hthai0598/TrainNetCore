import React from 'react'
import { Link } from 'react-router-dom'
import StatusTag from '../../elements/StatusTag'

export const columns = [
  {
    title: `Supplier's name`,
    key: 'name',
    render: record => {
      return <Link to={`/company-management/${record.id}`}>{record.name}</Link>
    },
  },
  {
    title: 'Request date',
    dataIndex: 'date',
    key: 'date',
  },
  {
    title: 'Status',
    key: 'status',
    dataIndex: 'status',
    render: status => {
      switch (status) {
        case 'completed':
          return <StatusTag mainColor='green'>{status}</StatusTag>
        case 'in progress':
          return <StatusTag mainColor='yellow'>{status}</StatusTag>
        case 'rejected':
          return <StatusTag mainColor='red'>{status}</StatusTag>
        case 'reviewal':
          return <StatusTag mainColor='blue'>{status}</StatusTag>
        case 'unseen':
          return <StatusTag mainColor='gray'>{status}</StatusTag>
      }
    },
  },
  {
    title: 'Email',
    dataIndex: 'email',
    key: 'email',
  },
  {
    title: 'Form',
    dataIndex: 'form',
    key: 'form',
  },
]
export const data = [
  {
    id: '1',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
  {
    id: '2',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'in progress',
  },
  {
    id: '3',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'rejected',
  },
  {
    id: '4',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'reviewal',
  },
  {
    id: '5',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'unseen',
  },
  {
    id: '6',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
  {
    id: '7',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'in progress',
  },
  {
    id: '8',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'rejected',
  },
  {
    id: '9',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'reviewal',
  },
  {
    id: '10',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'unseen',
  },
  {
    id: '11',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
  {
    id: '12',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'in progress',
  },
  {
    id: '13',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'rejected',
  },
  {
    id: '14',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'reviewal',
  },
  {
    id: '15',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'unseen',
  },
  {
    id: '16',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
  {
    id: '17',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'in progress',
  },
  {
    id: '18',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'rejected',
  },
  {
    id: '19',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'reviewal',
  },
  {
    id: '20',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'unseen',
  },
]