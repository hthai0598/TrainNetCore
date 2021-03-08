import React from 'react'
import StatusTag from '../../elements/StatusTag'

export const columns = [
  {
    title: `Supplier's name`,
    dataIndex: 'name',
    key: 'name',
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
    key: '1',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
  {
    key: '2',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'in progress',
  },
  {
    key: '3',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'rejected',
  },
  {
    key: '4',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'reviewal',
  },
  {
    key: '5',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'unseen',
  },
  {
    key: '6',
    name: 'John Brown',
    date: '13 Aug 2019',
    form: 'Form 1',
    email: 'supplier 1@gmail.com',
    status: 'completed',
  },
]