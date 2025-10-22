import { useEffect, useState } from 'react'
import { useSearchParams } from 'react-router-dom'
import { authService } from '../services/authService'

type Status = 'idle' | 'loading' | 'success' | 'error'

export default function VerifyEmail() {
  const [searchParams] = useSearchParams()
  const [status, setStatus] = useState<Status>('idle')
  const [message, setMessage] = useState<string>('E-postanız doğrulanıyor...')

  useEffect(() => {
    const userId = searchParams.get('userId')
    const token = searchParams.get('token')

    if (!userId || !token) {
      setStatus('error')
      setMessage('Geçersiz doğrulama bağlantısı. Parametreler eksik.')
      return
    }

    const verify = async () => {
      setStatus('loading')
      const res = await authService.verifyEmail(userId, token)

      if (!res) {
        setStatus('error')
        setMessage('Sunucudan yanıt alınamadı.')
        return
      }

      if (res.data?.success) {
        setStatus('success')
        setMessage('E-posta başarıyla doğrulandı.')
      } else {
        setStatus('error')
        setMessage(res.data?.message || 'Doğrulama başarısız.')
      }
    }

    verify()
  }, [searchParams])

  return (
    <div style={{ maxWidth: 520, margin: '40px auto', padding: 24 }}>
      <h2>E-posta Doğrulama</h2>
      <p>{message}</p>
      {status === 'loading' && <p>Lütfen bekleyin...</p>}
    </div>
  )
}